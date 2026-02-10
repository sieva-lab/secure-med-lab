import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { NgOptimizedImage } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Authentication } from '@securemed-app/authentication/authentication';
import { Authenticated } from '@securemed-app/authentication/authenticated';
import { Anonymous } from '@securemed-app/authentication/anonymous';
import ThemeToggle from '@securemed-app/core/theme/theme-toggle';

@Component({
	selector: 'securemed-header',
	imports: [RouterLink, Authenticated, Anonymous, RouterLinkActive, ThemeToggle, NgOptimizedImage],
	templateUrl: './header.html',
	styleUrl: './header.css',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class Header {
	private readonly authenticationService = inject(Authentication);
	protected readonly user = this.authenticationService.user;
}
