import { JsonPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Authentication } from '@securemed-app/authentication/authentication';
import { InfoCard } from '@securemed-app/shared/components/info-card/info-card';

@Component({
	selector: 'securemed-user',
	imports: [JsonPipe, InfoCard],
	templateUrl: './user.html',
	styleUrl: './user.css',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class User {
	private readonly userService = inject(Authentication);
	protected readonly user = this.userService.user;
}
