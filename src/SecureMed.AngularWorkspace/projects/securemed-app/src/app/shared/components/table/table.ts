import { ChangeDetectionStrategy, Component, TemplateRef, contentChild, input } from '@angular/core';
import { type HttpResourceRef } from '@angular/common/http';
import { NgTemplateOutlet } from '@angular/common';
import { TableBodyTemplate } from '@securemed-app/shared/components/table/table-body-template';

@Component({
	selector: 'securemed-table',
	imports: [NgTemplateOutlet],
	templateUrl: './table.html',
	styleUrl: './table.css',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Table {
	public readonly resource = input.required<HttpResourceRef<{ id: string | number }[] | undefined>>();
	public readonly headerTemplate = contentChild<TemplateRef<unknown>>('securemedTableHeader');
	public readonly footerTemplate = contentChild<TemplateRef<unknown>>('securemedTableFooter');
	public readonly bodyTemplate = contentChild(TableBodyTemplate, {
		read: TemplateRef,
	});

	protected refresh() {
		this.resource().reload();
	}
}
