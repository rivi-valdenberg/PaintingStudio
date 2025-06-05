import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ItemService } from '../../../services/item.service';
import { Item } from '../../../models/item.model';

@Component({
  selector: 'app-item-form',
  templateUrl: './item-form.component.html',
  styleUrls: ['./item-form.component.scss']
})
export class ItemFormComponent implements OnInit {
  itemForm: FormGroup;
  isEditMode = false;
  itemId: number | null = null;
  loading = false;
  submitting = false;
  imagePreview: string | null = null;
  selectedFile: File | null = null;
  
  constructor(
    private fb: FormBuilder,
    private itemService: ItemService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.itemForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.itemId = +id;
      this.loadItem(this.itemId);
    }
  }

  loadItem(id: number): void {
    this.loading = true;
    this.itemService.getItem(id).subscribe(
      (item) => {
        this.itemForm.patchValue({
          name: item.name,
          description: item.description
        });
        
        this.imagePreview = item.imageUrl;
        this.loading = false;
      },
      (error) => {
        console.error('Error loading item:', error);
        this.loading = false;
        this.router.navigate(['/items']);
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
    if (this.itemForm.invalid) {
      return;
    }
    
    this.submitting = true;
    
    const itemData = {
      name: this.itemForm.value.name,
      description: this.itemForm.value.description,
      image: this.selectedFile
    };
    
    if (this.isEditMode && this.itemId) {
      this.itemService.updateItem(this.itemId, itemData).subscribe(
        () => {
          this.submitting = false;
          this.router.navigate(['/items', this.itemId]);
        },
        (error) => {
          console.error('Error updating item:', error);
          this.submitting = false;
          alert('Failed to update item. Please try again.');
        }
      );
    } else {
      this.itemService.createItem(itemData).subscribe(
        (newItem) => {
          this.submitting = false;
          this.router.navigate(['/items', newItem.id]);
        },
        (error) => {
          console.error('Error creating item:', error);
          this.submitting = false;
          alert('Failed to create item. Please try again.');
        }
      );
    }
  }

  cancel(): void {
    if (this.isEditMode && this.itemId) {
      this.router.navigate(['/items', this.itemId]);
    } else {
      this.router.navigate(['/items']);
    }
  }
}