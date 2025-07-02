import { Routes } from '@angular/router';
import { AuthGuard } from './services/auth-guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/home/home.component').then(m => m.HomeComponent),
    canActivate: [AuthGuard],
    title: 'Home'
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent),
    title: 'Login'
  },
  {
    path: 'settings',
    loadComponent: () => import('./components/settings/settings.component').then(m => m.SettingsComponent),
    canActivate: [AuthGuard],
    title: 'Settings'
  },
  {
    path: 'home',
    redirectTo: '/',
    pathMatch: 'full'
  },
  {
    path: '**',
    loadComponent: () => import('./components/not-found/not-found.component').then(m => m.NotFoundComponent),
    title: 'Page Not Found'
  }
];
