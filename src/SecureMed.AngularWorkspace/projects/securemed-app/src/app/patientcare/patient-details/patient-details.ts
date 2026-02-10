import { ChangeDetectionStrategy, Component, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Patients } from '@securemed-app/patientcare/patientcare';
import { stronglyTypedIdAttribute } from '@securemed-app/shared/functions';
import { PatientId } from '@securemed-app/patientcare/models';
import { InfoCard } from '@securemed-app/shared/components/info-card/info-card';

@Component({
	selector: 'securemed-patient-details',
	imports: [RouterLink, InfoCard],
	templateUrl: './patient-details.html',
	styleUrl: './patient-details.css',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class PatientDetails {
	private readonly patientsService = inject(Patients);

	protected readonly patientId = input.required({ transform: stronglyTypedIdAttribute(PatientId) });
	protected readonly patient = this.patientsService.getPatientDetails(this.patientId);
}
