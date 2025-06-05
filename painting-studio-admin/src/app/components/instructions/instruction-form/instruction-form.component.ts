import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { InstructionService } from '../../../services/instruction.service';
import { ItemService } from '../../../services/item.service';
import { Instruction } from '../../../models/instruction.model';
import { Item } from '../../../models/item.model';

@Component({
  selector: 'app-instruction-form',
  templateUrl: './instruction-form.component.html',
  styleUrls: ['./instruction-form.component.scss']
})
export class InstructionFormComponent implements OnInit {
  instructionForm: FormGroup;
  isEditMode = false;
  instructionId: number | null = null;
  itemId: number;
  item: Item | null = null;
  loading = false;
  submitting = false;
  imagePreview: string | null = null;
  selectedFile: File | null = null;
  
  constructor(
    private fb: FormBuilder,
    private instructionService: InstructionService,
    private itemService: ItemService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.itemId = +this.route.snapshot.paramMap.get('itemId')!;
    
    this.instructionForm = this.fb.group({
      stepNumber: ['', [Validators.required]],
      description: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.loadItem();
    
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.instructionId = +id;
      this.loadInstruction(this.instructionId);
    }
  }

  loadItem(): void {
    this.itemService.getItem(this.itemId).subscribe(
      (item) => {
        this.item = item;
      },
      (error) => {
        console.error('Error loading item:', error);
        this.router.navigate(['/items']);
      }
    );
  }

  loadInstruction(id: number): void {
    this.loading = true;
    this.instructionService.getInstruction(id).subscribe(
      (instruction) => {
        if (instruction.itemId !== this.itemId) {
          this.router.navigate(['/items', this.itemId, 'instructions']);
          return;
        }
        
        this.instructionForm.patchValue({
          stepNumber: instruction.stepNumber,
          description: instruction.description
        });
        
        this.imagePreview = instruction.imageUrl;
        this.loading = false;
      },
      (error) => {
        console.error('Error loading instruction:', error);
        this.loading = false;
        this.router.navigate(['/items', this.itemId, 'instructions']);
      }
    );
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    
    if (input.files && input.files.length) {
      this.selectedFile = input.files[0];
      
      // Create image preview
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  onSubmit(): void {
    if (this.instructionForm.invalid) {
      return;
    }
    
    this.submitting = true;
    
    if (this.isEditMode && this.instructionId) {
      const instructionData = {
        stepNumber: this.instructionForm.value.stepNumber,
        description: this.instructionForm.value.description,
        image: this.selectedFile
      };
      
      this.instructionService.updateInstruction(this.instructionId, instructionData).subscribe(
        () => {
          this.submitting = false;
          this.router.navigate(['/items', this.itemId, 'instructions']);
        },
        (error) => {
          console.error('Error updating instruction:', error);
          this.submitting = false;
          alert('Failed to update instruction. Please try again.');
        }
      );
    } else {
      const instructionData = {
        itemId: this.itemId,
        stepNumber: this.instructionForm.value.stepNumber,
        description: this.instructionForm.value.description,
        image: this.selectedFile
      };
      
      this.instructionService.createInstruction(instructionData).subscribe(
        () => {
          this.submitting = false;
          this.router.navigate(['/items', this.itemId, 'instructions']);
        },
        (error) => {
          console.error('Error creating instruction:', error);
          this.submitting = false;
          alert('Failed to create instruction. Please try again.');
        }
      );
    }
  }

  cancel(): void {
    this.router.navigate(['/items', this.itemId, 'instructions']);
  }
}