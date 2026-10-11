import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import GamesClient from '../clients/GamesClient';
import GenresClient from '../clients/GenresClient';
import { GameDetails } from '../models/GameDetails';
import { Genre } from '../models/Genre';

const EditGame: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [game, setGame] = useState<GameDetails | null>(null);
    const [genres, setGenres] = useState<Genre[] | null>(null);
    const [title, setTitle] = useState<string>('');
    const [loadingErrorList, setLoadingErrorList] = useState<string[]>([]);
    const [errorList, setErrorList] = useState<string[]>([]);
    const genresClient = new GenresClient();
    const defaultImageUri = 'https://placehold.co/100';

    useEffect(() => {
        const fetchData = async () => {
            try {
                const gamesClient = new GamesClient();

                if (id) {
                    const gameData = await gamesClient.getGameAsync(id);
                    setGame(gameData);
                    setTitle(`Edit ${gameData.name}`);

                } else {
                    setGame({
                        id: '',
                        name: '',
                        genreId: null,
                        price: 0,
                        releaseDate: new Date().toISOString().split('T')[0],
                        description: '',
                        imageUri: defaultImageUri,
                    });
                    setTitle('New Game');
                }

                const genresData = await genresClient.getGenresAsync();
                setGenres(genresData);

            } catch (error: unknown) {
                if (error instanceof Error) {
                    setLoadingErrorList([error.message]);
                } else {
                    setLoadingErrorList(['An unknown error occurred']);
                }
            }
        };
        fetchData();
    }, [id]);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (!game) return;

        setErrorList([]);

        const gamesClient = new GamesClient();

        let result;
        if (!id) {
            result = await gamesClient.addGameAsync(game);
        } else {
            game.id = id;
            result = await gamesClient.updateGameAsync(game);
        }

        if (result.succeeded) {
            navigate('/');
        } else {
            setErrorList(result.errors);
        }
    };

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
        const { name, value } = event.target;
        setGame((prevGame) => ({
            ...prevGame!,
            [name]: name === 'price' ? parseFloat(value) : value,
        }));
    };

    if (loadingErrorList.length > 0) {
        return <div>
            {loadingErrorList.map((error, index) => (
                <div key={index} className="mt-3 text-danger">
                    <em>{error}</em>
                </div>
            ))}
        </div>
    }

    if (!genres || !game) {
        return <p className="mt-3"><em>Loading...</em></p>;
    }

    return (
        <div>
            <h3>{title}</h3>
            {errorList.length > 0 && (
                <div className="modal-body mt-3">
                    {errorList.map((error, index) => (
                        <div key={index} className="alert alert-danger">
                            {error}
                        </div>
                    ))}
                </div>
            )}
            <div className="row mb-2">
                <div className="col-md-4">
                    <form onSubmit={handleSubmit} encType="multipart/form-data">
                        <div className="mb-3">
                            <label htmlFor="name" className="form-label">Name:</label>
                            <input
                                id="name"
                                name="name"
                                type="text"
                                value={game.name}
                                onChange={handleInputChange}
                                className="form-control"
                                required
                            />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="genre" className="form-label">Genre:</label>
                            <select
                                id="genre"
                                name="genreId"
                                value={game.genreId || ''}
                                onChange={handleInputChange}
                                className="form-select"
                                required
                            >
                                <option value="">Select a genre</option>
                                {genres.map((genre) => (
                                    <option key={genre.id} value={genre.id}>{genre.name}</option>
                                ))}
                            </select>
                        </div>
                        <div className="mb-3">
                            <label htmlFor="price" className="form-label">Price:</label>
                            <input
                                id="price"
                                name="price"
                                type="number"
                                value={game.price}
                                onChange={handleInputChange}
                                className="form-control"
                                required
                                min="1"
                                max="100"
                                step="0.01"
                            />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="releaseDate" className="form-label">Release Date:</label>
                            <input
                                id="releaseDate"
                                name="releaseDate"
                                type="date"
                                value={game.releaseDate}
                                onChange={handleInputChange}
                                className="form-control"
                                required
                            />
                        </div>
                        <button type="submit" className="btn btn-primary me-1">Save</button>
                        <button type="button" className="btn btn-secondary" onClick={() => navigate('/')}>Cancel</button>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default EditGame;