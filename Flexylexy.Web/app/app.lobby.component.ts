import { Component } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { GameService } from "./services/game.service";
import { UserService } from "./services/user.service";
import { ChatService } from "./services/chat.service";

declare var $: any;

@Component({
    selector: 'lobby',
    templateUrl: './Pages/app.lobby.component.html'
})
export class LobbyComponent {
    public players;
    private subscriptions = [];

    public chatroom: string;

    constructor(
        private router: Router,
        private _gameService: GameService,
        private _userService: UserService,
        private _chatService: ChatService) { }

    ngOnInit() {
        this.subscribeToEvents();
        
        this._gameService.getPlayers();
    }

    private subscribeToEvents() {
        var self = this;

        var subscription = this._gameService.playersUpdated.subscribe(players => this.players = players);
        this.subscriptions.push(subscription);

        subscription = this._gameService.challenged.subscribe((player) => {
            var accepted = confirm("You have been challenged to a game by " + player.Name);

            if (accepted) {
                this._gameService.acceptChallenge(player);
                this.router.navigate(['/tictactoe', player.ConnectionToken, false]);
            } else {
                this._gameService.declineChallenge(player);
            }
        });
        this.subscriptions.push(subscription);

        subscription = this._gameService.challengeAccepted.subscribe((player) => {
            alert("Challenge accepted.");
            this.router.navigate(['/tictactoe', player.ConnectionToken, true]);
        });
        this.subscriptions.push(subscription);

        subscription = this._gameService.challengeDeclined.subscribe(() => alert("Challenge declined."));
        this.subscriptions.push(subscription);

        subscription = this._chatService.receivedChat.subscribe(x => {
            this.chatroom = x.sender.Name + ": " + x.message + "<br/>" + this.chatroom;
        });
        this.subscriptions.push(subscription);
    }

    public challenge(player) {
        this._gameService.challenge(player);
    }

    public sendChat(message) {
        this._chatService.sendChat(message);
    }

    ngOnDestroy() {
        this.subscriptions.forEach(x => x.unsubscribe());
    }
}