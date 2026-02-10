import { type Routes } from '@angular/router';
import { authenticatedGuard } from '@securemed-app/authentication/authenticated-guard';

export const routes: Routes = [
	{
		path: '',
		canActivateChild: [authenticatedGuard],
		children: [
			{
				path: 'patients',
				loadChildren: () => import('@securemed-app/patientcare/patientcare.routes'),
				title: 'Patients',
			},
		],
	},
	{
		path: 'user',
		loadComponent: () => import('@securemed-app/authentication/user/user'),
		title: 'User',
	},
	{
		path: '**',
		loadComponent: () => import('@securemed-app/authentication/not-found/not-found'),
		title: 'Not Found',
	},
];
