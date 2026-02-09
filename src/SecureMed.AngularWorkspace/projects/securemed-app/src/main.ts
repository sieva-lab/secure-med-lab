import { App } from '@securemed-app/app';
import { appConfig } from '@securemed-app/app.config';
import { bootstrapApplication } from '@angular/platform-browser';

bootstrapApplication(App, appConfig).catch((err: unknown) => {
	console.error(err);
});
