import { Component } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { GameService } from "./services/game.service";
import { Player } from "./types/player";
import { Move } from "./types/move";

declare var $: any;

@Component({
    selector: 'tictactoe',
    templateUrl: './Pages/app.tictactoe.component.html'
})

export class TicTacToeComponent {
    private isMyBoard;
    private isMyTurn;
    private myMark;
    private opponentMark;
    private _opponentConnectionToken: string;

    public board = [];

    constructor(
        private _route: ActivatedRoute,
        private _router: Router,
        private _gameService: GameService) { }

    ngOnInit() {
        this.readFromRoute();
        this.registerForClient();
    }

    private readFromRoute() {
        this._route.params.forEach((params: Params) => {
            let isChallenger = params["isChallenger"] == "true";
            this._opponentConnectionToken = params["opponentConnectionToken"];

            this.isMyBoard = isChallenger;
            this.isMyTurn = isChallenger;
            this.myMark = { value: 1, mark: isChallenger ? "X" : "O" };
            this.opponentMark = { value: 4, mark: isChallenger ? "O" : "X" };
        });
    }

    private registerForClient() {
        var self = this;

        this._gameService.opponentPlay.subscribe((move: Move) => {
            if (move.Player.ConnectionToken != self._opponentConnectionToken) return;

            if (self.isMyTurn) return;

            if (!self.board[move.Position]) {
                self.board[move.Position] = self.opponentMark;
                if (self.isOpponentWin()) {
                    alert("You lost.");
                    this._router.navigate(['\lobby']);
                }
            }

            self.isMyTurn = !self.isMyTurn;
        });
    }

    public play(position) {
        if (!this.isMyTurn) return;

        if (!this.board[position]) {
            this.board[position] = this.myMark;

            var move = new Move();
            move.Position = position;
            move.Player = new Player();
            move.Player.ConnectionToken = this._opponentConnectionToken;
            
            this._gameService.play(move);

            if (this.isMyWin()) {
                alert("You win!");
                this._router.navigate(['\lobby']);
            }
        }

        this.isMyTurn = !this.isMyTurn;
    }

    private isMyWin() {
        return this.checkValue(3);
    }

    private isOpponentWin() {
        return this.checkValue(12);
    }

    private checkValue(value) {
        return this.sum(0, 1, 2) === value ||
            this.sum(3, 4, 5) === value ||
            this.sum(6, 7, 8) === value ||
            this.sum(0, 3, 6) === value ||
            this.sum(1, 4, 7) === value ||
            this.sum(2, 5, 8) === value ||
            this.sum(0, 4, 8) === value ||
            this.sum(2, 4, 6) === value;
    }

    private sum(index1, index2, index3) {
        return (!!this.board[index1] ? this.board[index1].value : 0) +
            (!!this.board[index2] ? this.board[index2].value : 0) +
            (!!this.board[index3] ? this.board[index3].value : 0);
    }
}