import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ItemService } from '../../../services/item.service';
import { Item } from '../../../models/item.model';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.scss']
})
export class ItemListComponent implements OnInit {
  items: Item[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private itemService: ItemService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.loading = true;
    this.itemService.getItems().subscribe(
      (data) => {
        this.items = data;
        this.loading = false;
      },
      (error) => {
        this.error = 'Failed to load items. Please try again.';
        this.loading = false;
        console.error('Error loading items:', error);
      }
    );
  }

  editItem(id: number): void {
    this.router.navigate(['/items', id, 'edit']);
  }

  viewItem(id: number): void {
    this.router.navigate(['/items', id]);
  }

  deleteItem(id: number): void {
    if (confirm('Are you sure you want to delete this item? This action cannot be undone.')) {
      this.itemService.deleteItem(id).subscribe(
        () => {
          this.items = this.items.filter(item => item.id !== id);
        },
        (error) => {
          console.error('Error deleting item:', error);
          alert('Failed to delete item. Please try again.');
        }
      );
    }
  }

  createNewItem(): void {
    this.router.navigate(['/items/new']);
  }
}