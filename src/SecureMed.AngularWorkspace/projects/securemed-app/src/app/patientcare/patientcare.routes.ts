import { type Routes } from '@angular/router';

export const patientCareRoutes: Routes = [
	{
		path: '',
		children: [
			{
				path: '',
				loadComponent: () => import('./patients-overview/patients-overview'),
			},
			{
				path: ':patientId',
				loadComponent: () => import('./patient-details/patient-details'),
			},
			{
				path: '**',
				redirectTo: '',
			},
		],
	},
];

export default patientCareRoutes;
