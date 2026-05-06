export interface Tag {
  tagId: number | string;
  name: string;
  slug: string;
}

export interface Note {
  noteId: number | string;
  title: string;
  body: string;
  slug: string;
  tags: Tag[];
}
