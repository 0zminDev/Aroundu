import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, signal } from '@angular/core';

@Component({
	selector: 'app-root',
	imports: [CommonModule],
	templateUrl: './app.html',
	styleUrl: './app.scss'
})
export class App {
	protected readonly event = signal<{message: string} | null>(null);

	constructor(private http: HttpClient) {}

	ngOnInit() {
		this.http.get<{message: string}>('/api/events').subscribe(data => {
			this.event.set(data);
		});
	}
}
