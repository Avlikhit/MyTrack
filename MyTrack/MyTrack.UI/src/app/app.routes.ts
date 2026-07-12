import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/dashboard/dashboard';
import { Projects } from './features/projects/projects';
import { Worklogs } from './features/worklogs/worklogs';
import { WorklogDetails } from './features/worklogs/worklog-details/worklog-details';
import { ProjectDetails } from './features/projects/project-details/project-details';
import { PayInformation } from './features/pay-information/pay-information';
import { PayrollSettings } from './features/payroll-settings/payroll-settings';
import { PayDetails } from './features/pay-information/pay-details/pay-details';
import { Profile } from './features/profile/profile';

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
    component: Dashboard,
    canActivate: [authGuard]
  },
  {
    path: 'projects',
    component: Projects,
    canActivate: [authGuard]
  },
  {
    path: 'worklogs',
    component: Worklogs,
    canActivate: [authGuard]
  },
  {
    path: 'worklogs/:id',
    component: WorklogDetails,
    canActivate: [authGuard]
  },
  {
    path: 'projects/:id',
    component: ProjectDetails,
    canActivate: [authGuard]
  },
  {
    path: 'pay-information',
    component: PayInformation,
    canActivate: [authGuard]
  },
  {
    path: 'pay-information/:id',
    component: PayDetails,
    canActivate: [authGuard]
  },
  {
    path: 'profile',
    component: Profile,
    canActivate: [authGuard]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
