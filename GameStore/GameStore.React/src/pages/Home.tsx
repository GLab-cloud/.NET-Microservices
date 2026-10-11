import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import GamesClient from '../clients/GamesClient';
import { GameSummary } from '../models/GameSummary';
import DeleteGameModal from '../components/DeleteGameModal';

// Declare bootstrap property on window object
declare global {
    interface Window {
        bootstrap: any;
    }
}

const Home: React.FC = () => {
    const [games, setGames] = useState<GameSummary[]>([]);
    const [loadingErrorList, setLoadingErrorList] = useState<string[]>([]);
    const [errorList, setErrorList] = useState<string[]>([]);
    const [gameToDelete, setGameToDelete] = useState<GameSummary | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const client = new GamesClient();

    const fetchGames = async () => {
        try {
            setIsLoading(true);
            const response = await client.getGamesAsync();
            setGames(response);
            setLoadingErrorList([]);
        } catch (error: unknown) {
            if (error instanceof Error) {
                setLoadingErrorList([error.message]);
            } else {
                setLoadingErrorList(['An unknown error occurred']);
            }
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        document.title = 'Game Store';
        fetchGames();
    }, []);

    useEffect(() => {
        if (gameToDelete) {
            const modalElement = document.getElementById(`deleteModal-${gameToDelete.id}`)!;
            const modal = new window.bootstrap.Modal(modalElement);

            // Listen for modal hide event to reset gameToDelete
            const handleModalHide = () => {
                setGameToDelete(null);
            };

            modalElement.addEventListener('hidden.bs.modal', handleModalHide);
            modal.show();

            // Cleanup event listener
            return () => {
                modalElement.removeEventListener('hidden.bs.modal', handleModalHide);
            };
        }
    }, [gameToDelete]);

    const handleDelete = async (gameId: string) => {
        setErrorList([]);
        try {
            const result = await client.deleteGameAsync(gameId);

            if (result.succeeded) {
                setGameToDelete(null); // Reset the modal state
                fetchGames();
            } else {
                setErrorList(result.errors);
            }
        } catch (error: unknown) {
            if (error instanceof Error) {
                setErrorList([error.message]);
            } else {
                setErrorList(['An unknown error occurred']);
            }
        }
    };

    if (isLoading) {
        return <p className="mt-3"><em>Loading...</em></p>;
    }

    if (loadingErrorList.length > 0) {
        return (
            <div>
                <div className="row mt-2">
                    <div className="col">
                        <Link className="btn btn-primary" to="/editgame" role="button">
                            New Game
                        </Link>
                    </div>
                </div>
                {loadingErrorList.map((error, index) => (
                    <div key={index} className="mt-3 text-danger">
                        <em>Error loading games: {error}</em>
                    </div>
                ))}
            </div>
        );
    }

    return (
        <div>
            <div className="row mt-2">
                <div className="col">
                    <Link className="btn btn-primary" to="/editgame" role="button">
                        New Game
                    </Link>
                </div>
            </div>

            {errorList.length > 0 && (
                <div className="modal-body mt-3">
                    {errorList.map((error, index) => (
                        <div key={index} className="alert alert-danger">
                            {error}
                        </div>
                    ))}
                </div>
            )}

            {games.length === 0 ? (
                <div className="mt-3">
                    <p><em>No games found. Click "New Game" to add your first game!</em></p>
                </div>
            ) : (
                <table className="table table-striped table-bordered table-hover mt-3">
                    <thead className="table-dark">
                        <tr>
                            <th>Name</th>
                            <th>Genre</th>
                            <th className="text-end">Price</th>
                            <th>Release Date</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {games.map((game) => (
                            <tr key={game.id}>
                                <td>{game.name}</td>
                                <td>{game.genre}</td>
                                <td className="text-end">${game.price}</td>
                                <td>{game.releaseDate}</td>
                                <td>
                                    <div className="d-flex">
                                        <Link className="btn btn-primary me-2" to={`/editgame/${game.id}`} role="button">
                                            <i className="bi bi-pencil"></i>
                                        </Link>
                                        <button className="btn btn-danger" onClick={() => setGameToDelete(game)}>
                                            <i className="bi bi-x-lg"></i>
                                        </button>
                                    </div>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}

            {/* Delete Confirmation Modal */}
            {gameToDelete && (
                <DeleteGameModal game={gameToDelete} onDelete={handleDelete} />
            )}
        </div>
    );
};

export default Home;