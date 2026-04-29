export interface CommitmentFrequency { frequencyId: number; frequencyTypeId: number; }
export interface Commitment { commitmentId: number; behaviourId: number; profileId: number; commitmentFrequencies: CommitmentFrequency[]; }
