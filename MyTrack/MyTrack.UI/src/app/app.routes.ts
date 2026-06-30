import { Routes } from '@angular/router';

import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/dashboard/dashboard';
import { Projects } from './features/projects/projects';
import { Worklogs } from './features/worklogs/worklogs';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: Login
  },
  {
    path: 'register',
    component: Register
  },
  {
    path: 'dashboard',
    component: Dashboard
  },
  {
    path: 'projects',
    component: Projects
  },
  {
    path: 'worklogs',
    component: Worklogs
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
