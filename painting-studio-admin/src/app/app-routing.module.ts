import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Components
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ItemListComponent } from './components/items/item-list/item-list.component';
import { ItemFormComponent } from './components/items/item-form/item-form.component';
import { ItemDetailComponent } from './components/items/item-detail/item-detail.component';
import { InstructionListComponent } from './components/instructions/instruction-list/instruction-list.component';
import { InstructionFormComponent } from './components/instructions/instruction-form/instruction-form.component';
import { LoginComponent } from './components/auth/login/login.component';

// Guards
import { AuthGuard } from './components/auth/auth.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'items', component: ItemListComponent, canActivate: [AuthGuard] },
  { path: 'items/new', component: ItemFormComponent, canActivate: [AuthGuard] },
  { path: 'items/:id', component: ItemDetailComponent, canActivate: [AuthGuard] },
  { path: 'items/:id/edit', component: ItemFormComponent, canActivate: [AuthGuard] },
  { path: 'items/:itemId/instructions', component: InstructionListComponent, canActivate: [AuthGuard] },
  { path: 'items/:itemId/instructions/new', component: InstructionFormComponent, canActivate: [AuthGuard] },
  { path: 'items/:itemId/instructions/:id/edit', component: InstructionFormComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }