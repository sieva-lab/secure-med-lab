import { Injectable, type Signal, inject } from '@angular/core';
import { HttpClient, type HttpResourceRef, httpResource } from '@angular/common/http';
import { type Observable } from 'rxjs';
import { parse, parseCollection } from '@securemed-app/shared/functions';
import {
    type CreatePatientCommand,
    PatientDetailsResponse,
    type  PatientId,
    PatientOverviewResponse,
} from '@securemed-app/patientcare/models';

@Injectable({
	providedIn: 'root',
})
export class Patients {
	private http = inject(HttpClient);

	public getOverview(): HttpResourceRef<PatientOverviewResponse[] | undefined> {
		return httpResource(() => '/api/patients', {
			parse: parseCollection(PatientOverviewResponse),
		});
	}

	public getPatientDetails(id: Signal<PatientId>): HttpResourceRef<PatientDetailsResponse | undefined> {
		return httpResource(
			() => ({
				url: `/api/patients/${id()}`,
			}),
			{
				parse: parse(PatientDetailsResponse),
			},
		);
	}

	public createPatient(patient: CreatePatientCommand): Observable<PatientId> {
		return this.http.post<PatientId>('/api/patients', patient);
	}
}
