import React from 'react';
import { GameSummary } from '../models/GameSummary';

interface DeleteGameModalProps {
  game: GameSummary;
  onDelete: (gameId: string) => void;
}

const DeleteGameModal: React.FC<DeleteGameModalProps> = ({ game, onDelete }) => {
  const modalId = `deleteModal-${game.id}`;
  const title = `Delete ${game.name}?`;

  const handleDelete = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onDelete(game.id);
  };

  return (
    <div className="modal fade" id={modalId} tabIndex={-1} aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h1 className="modal-title fs-5" id="exampleModalLabel">{title}</h1>
            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
            <form onSubmit={handleDelete}>
              <button type="submit" className="btn btn-primary" data-bs-dismiss="modal">
                Delete
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DeleteGameModal;