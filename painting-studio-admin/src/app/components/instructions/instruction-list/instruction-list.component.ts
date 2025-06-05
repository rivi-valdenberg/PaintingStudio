import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InstructionService } from '../../../services/instruction.service';
import { ItemService } from '../../../services/item.service';
import { Instruction } from '../../../models/instruction.model';
import { Item } from '../../../models/item.model';

@Component({
  selector: 'app-instruction-list',
  templateUrl: './instruction-list.component.html',
  styleUrls: ['./instruction-list.component.scss']
})
export class InstructionListComponent implements OnInit {
  instructions: Instruction[] = [];
  item: Item | null = null;
  itemId: number;
  loading = true;
  error: string | null = null;

  constructor(
    private instructionService: InstructionService,
    private itemService: ItemService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.itemId = +this.route.snapshot.paramMap.get('itemId')!;
  }

  ngOnInit(): void {
    this.loadItem();
    this.loadInstructions();
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

  loadInstructions(): void {
    this.loading = true;
    this.instructionService.getInstructionsByItem(this.itemId).subscribe(
      (data) => {
        this.instructions = data;
        this.loading = false;
      },
      (error) => {
        this.error = 'Failed to load instructions. Please try again.';
        this.loading = false;
        console.error('Error loading instructions:', error);
      }
    );
  }

  editInstruction(id: number): void {
    this.router.navigate(['/items', this.itemId, 'instructions', id, 'edit']);
  }

  deleteInstruction(id: number): void {
    if (confirm('Are you sure you want to delete this instruction? This action cannot be undone.')) {
      this.instructionService.deleteInstruction(id).subscribe(
        () => {
          this.instructions = this.instructions.filter(instruction => instruction.id !== id);
        },
        (error) => {
          console.error('Error deleting instruction:', error);
          alert('Failed to delete instruction. Please try again.');
        }
      );
    }
  }

  createNewInstruction(): void {
    this.router.navigate(['/items', this.itemId, 'instructions', 'new']);
  }

  backToItem(): void {
    this.router.navigate(['/items', this.itemId]);
  }
}