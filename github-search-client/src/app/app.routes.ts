import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { SearchComponent } from './components/search/search';
import { BookmarksComponent } from './components/bookmarks/bookmarks';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'search', component: SearchComponent, canActivate: [authGuard] },
  { path: 'bookmarks', component: BookmarksComponent, canActivate: [authGuard] },
  { path: '', redirectTo: 'search', pathMatch: 'full' },
  { path: '**', redirectTo: 'search' }
];