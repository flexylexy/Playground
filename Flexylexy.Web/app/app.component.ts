import { Component } from '@angular/core';
import { GameService } from "./services/game.service";
import { UserService } from "./services/user.service";
import { HubService } from "./services/hub.service";

@Component({
    selector: 'app',
    templateUrl: './Pages/app.html'
})
export class AppComponent {
    public name: string;

    constructor(
        private _hubService: HubService,
        private _gameService: GameService,
        private _userService: UserService
    ) {
        this._userService.nameObservable.subscribe(name => this.name = name);
    }

    ngOnInit() {
        this._hubService.connect();
    }

    public clearPlayers() {
        this._gameService.clearPlayers();
    }
}