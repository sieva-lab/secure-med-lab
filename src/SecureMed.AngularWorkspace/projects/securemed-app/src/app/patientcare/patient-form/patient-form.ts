import { ChangeDetectionStrategy, Component, inject, output, signal } from '@angular/core';
import {
	Field,
	type OneOrMany,
	type ValidationError,
	type WithOptionalField,
	applyWhen,
	form,
	hidden,
	maxLength,
	minLength,
	required,
	schema,
	submit,
} from '@angular/forms/signals';
import { type Address, type PatientId } from '@securemed-app/patientcare/models';
import { Patients } from '@securemed-app/patientcare/patientcare';
import { PatientAddress } from '@securemed-app/patientcare/shared/patient-address/patient-address';
import { HttpErrorResponse } from '@angular/common/http';
import { type Observable, catchError, firstValueFrom, map, of } from 'rxjs';
import { formValidation } from '@form-validation';
import { Alert } from '@securemed-app/shared/components/alert/alert';

@Component({
	selector: 'securemed-patient-form',
	imports: [Field, PatientAddress, formValidation, Alert],
	templateUrl: './patient-form.html',
	styleUrl: './patient-form.css',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientForm {
	private readonly patientsService = inject(Patients);

	public readonly patientCreated = output<PatientId>();

	protected readonly patientForm = form(signal<PatientFormModel>(this.initializePatientModel()), (path) => {
		required(path.firstName);
		maxLength(path.firstName, 255);
		required(path.lastName);
		maxLength(path.lastName, 255);

		hidden(path.billingAddress, (logic) => !logic.valueOf(path.hasBillingAddress));
		applyWhen(path.billingAddress, (logic) => logic.valueOf(path.hasBillingAddress), addressSchema);

		hidden(path.shippingAddress, (logic) => !logic.valueOf(path.hasShippingAddress));
		applyWhen(path.shippingAddress, (logic) => logic.valueOf(path.hasShippingAddress), addressSchema);
		minLength(path.shippingAddress.note, (logic) => {
			return logic.value() ? 10 : 0;
		});
	});

	protected async onSubmit(): Promise<void> {
		await submit(this.patientForm, async (form) => {
			if (this.patientForm().invalid()) {
				return;
			}

			const formValue = form().value();
			return await firstValueFrom(
				this.patientsService
					.createPatient({
						firstName: formValue.firstName,
						lastName: formValue.lastName,
                        address: {
                            city: formValue.address.city,
                            street: formValue.address.street,
                            zipCode: formValue.address.zipCode,
                        },
						billingAddress: formValue.hasBillingAddress
							? {
									city: formValue.billingAddress.city,
									street: formValue.billingAddress.street,
									zipCode: formValue.billingAddress.zipCode,
								}
							: null,
						shippingAddress: formValue.hasShippingAddress
							? {
									city: formValue.shippingAddress.city,
									street: formValue.shippingAddress.street,
									zipCode: formValue.shippingAddress.zipCode,
									note: formValue.shippingAddress.note ? formValue.shippingAddress.note : null,
								}
							: null,
					})
					.pipe(
						map((patientId) => {
							form().reset();
							form().value.set(this.initializePatientModel());
							this.patientCreated.emit(patientId);
							return null;
						}),
						catchError((error: unknown): Observable<OneOrMany<WithOptionalField<ValidationError>>> => {
							return of({
								kind: 'server',
								message:
									error instanceof HttpErrorResponse && this.isErrorWithTitle(error.error)
										? error.error.title
										: 'An unexpected error occurred, please try again.',
							});
						}),
					),
			);
		});
	}

	private isErrorWithTitle(error: unknown): error is { title: string } {
		return typeof error === 'object' && error !== null && typeof (error as { title: unknown }).title === 'string';
	}

	private initializePatientModel(): PatientFormModel {
		return {
			firstName: '',
			lastName: '',
            address: {
                city: '',
				street: '',
				zipCode: '',
            },
			hasBillingAddress: false,
			billingAddress: {
				city: '',
				street: '',
				zipCode: '',
			},
			hasShippingAddress: false,
			shippingAddress: {
				city: '',
				street: '',
				zipCode: '',
				note: '',
			},
		};
	}
}

const addressSchema = schema<Address>((path) => {
	required(path.street);
	minLength(path.street, 3);
	required(path.city);
	minLength(path.city, 3);
	required(path.zipCode);
	minLength(path.zipCode, 3, { message: 'Zip code must be between 3 and 5 characters long.' });
	maxLength(path.zipCode, 5, { message: 'Zip code must be between 3 and 5 characters long.' });
});

interface PatientFormModel {
	firstName: string;
	lastName: string;
    address: {
        street: string;
		city: string;
		zipCode: string;
    };
	hasBillingAddress: boolean;
	billingAddress: {
		street: string;
		city: string;
		zipCode: string;
	};
	hasShippingAddress: boolean;
	shippingAddress: {
		street: string;
		city: string;
		zipCode: string;
		note: string;
	};
}
