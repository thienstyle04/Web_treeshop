import { Component, inject } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './shared/components/header/header.component';
import { FooterComponent } from './shared/components/footer/footer.component';
import { filter, map } from 'rxjs/operators';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent, CommonModule],
  template: `
    @if (!isAdminRoute()) {
      <app-header></app-header>
    }
    <main class="main-content" [class.admin-page]="isAdminRoute()">
      <router-outlet></router-outlet>
    </main>
    @if (!isAdminRoute()) {
      <app-footer></app-footer>
    }
  `,
  styles: [`
    :host {
      display: flex;
      flex-direction: column;
      min-height: 100vh;
    }

    .main-content {
      flex: 1;
    }

    .main-content.admin-page {
      display: flex;
      flex-direction: column;
    }
  `]
})
export class App {
  private router = inject(Router);

  isAdminRoute = toSignal(
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map((event: NavigationEnd) => event.urlAfterRedirects.startsWith('/admin'))
    ),
    { initialValue: window.location.pathname.startsWith('/admin') }
  );
}
