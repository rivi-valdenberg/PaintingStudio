import { Component, OnInit } from '@angular/core';
import { ItemService } from '../../services/item.service';
import { Item } from '../../models/item.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  items: Item[] = [];
  loading = true;
  error: string | null = null;
  
  constructor(private itemService: ItemService) { }
  
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
}