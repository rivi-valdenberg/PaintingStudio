export interface Instruction {
    id: number;
    itemId: number;
    stepNumber: string;
    description: string;
    imageUrl: string;
  }
  
  export interface InstructionCreate {
    itemId: number;
    stepNumber: string;
    description: string;
    image: File | null;
  }
  
  export interface InstructionUpdate {
    stepNumber: string;
    description: string;
    image: File | null;
  }