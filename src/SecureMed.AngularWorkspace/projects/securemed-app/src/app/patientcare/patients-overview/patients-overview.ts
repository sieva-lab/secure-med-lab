import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Patients } from '@securemed-app/patientcare/patientcare';
import { PatientForm } from '@securemed-app/patientcare/patient-form/patient-form';
import { PatientsList } from '@securemed-app/patientcare/patients-list/patients-list';


@Component({
	selector: 'securemed-patients-overview',
	imports: [PatientsList, PatientForm],
	templateUrl: './patients-overview.html',
	styleUrl: './patients-overview.css',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class PatientsOverview {
	private readonly patientsService = inject(Patients);
	protected readonly patients = this.patientsService.getOverview();

	protected patientCreated(): void {
		this.patients.reload();
	}
}
