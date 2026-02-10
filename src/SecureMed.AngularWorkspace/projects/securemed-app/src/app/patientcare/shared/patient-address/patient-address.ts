import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { Field, type FieldTree } from '@angular/forms/signals';
import { formValidation } from '@form-validation';
import { type Address } from '@securemed-app/patientcare/models';

@Component({
	selector: 'securemed-patient-address',
	imports: [Field, formValidation],
	templateUrl: './patient-address.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientAddress {
	public readonly address = input.required<FieldTree<Address>>();
}
