import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';  // ← הוסף את זה
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Components

import { ItemListComponent } from './components/items/item-list/item-list.component';
import { ItemFormComponent } from './components/items/item-form/item-form.component';
import { ItemDetailComponent } from './components/items/item-detail/item-detail.component';
import { InstructionListComponent } from './components/instructions/instruction-list/instruction-list.component';
import { InstructionFormComponent } from './components/instructions/instruction-form/instruction-form.component';
import { LoginComponent } from './components/auth/login/login.component';

// Services
import { AuthService } from './services/auth.service';
import { ItemService } from './services/item.service';
import { InstructionService } from './services/instruction.service';

// Interceptors
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    ItemListComponent,
    ItemFormComponent,
    ItemDetailComponent,
    InstructionListComponent,
    InstructionFormComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,        // ← הוסף את זה
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    AuthService,
    ItemService,
    InstructionService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
