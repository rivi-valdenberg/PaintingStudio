import { Instruction } from './instruction.model';

export interface Item {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  createdAt: string;
  instructions: Instruction[];
}

export interface ItemCreate {
  name: string;
  description: string;
  image: File | null;
}

export interface ItemUpdate {
  name: string;
  description: string;
  image: File | null;
}