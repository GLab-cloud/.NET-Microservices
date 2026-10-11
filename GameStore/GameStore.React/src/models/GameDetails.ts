export interface GameDetails {
    id: string;
    name: string;
    genreId: string | null;
    price: number;
    releaseDate: string;
    description: string;
    imageFile?: File | null;
    imageUri?: string | null;
}