import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstructionFormComponent } from './instruction-form.component';

describe('InstructionFormComponent', () => {
  let component: InstructionFormComponent;
  let fixture: ComponentFixture<InstructionFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstructionFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InstructionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
