import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import type { HttpResourceRef } from '@angular/common/http';
import type { PatientOverviewResponse } from '@securemed-app/patientcare/models/patient-overview-response.model';
import { Table } from '@securemed-app/shared/components/table/table';
import { TableBodyTemplate } from '@securemed-app/shared/components/table/table-body-template';

@Component({
	selector: 'securemed-patients-list',
	imports: [Table, TableBodyTemplate, RouterLink],
	templateUrl: './patients-list.html',
	styleUrl: './patients-list.css',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientsList {
	public readonly patients = input.required<HttpResourceRef<PatientOverviewResponse[] | undefined>>();
}
