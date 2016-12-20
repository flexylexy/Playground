import { Injectable, EventEmitter } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Subject } from "rxjs/Subject";
import { asObservable} from "../helpers/asObservable";
import { HubService } from "./hub.service";
import { UserService } from "./user.service";
import { Player } from "../types/player";
import { Move } from "../types/move";
 
declare var $: any;

@Injectable()
export class GameService {
    private _proxy;
    private _token;


    private _opponentPlaySubject: Subject<any> = new Subject();
    private _challengedSubject: Subject<any> = new Subject();
    private _challengeAcceptedSubject: Subject<any> = new Subject();
    private _challengeDeclinedSubject: Subject<any> = new Subject();
    private _playersUpdatedSubject: Subject<any> = new Subject();

    public opponentPlay: Observable<any> = asObservable(this._opponentPlaySubject);
    public challenged: Observable<any> = asObservable(this._challengedSubject);
    public challengeAccepted: Observable<any> = asObservable(this._challengeAcceptedSubject);
    public challengeDeclined: Observable<any> = asObservable(this._challengeDeclinedSubject);
    public playersUpdated: Observable<any> = asObservable(this._playersUpdatedSubject);

    constructor(
        private _userService: UserService,
        private _hubService: HubService
    ) {
        this._proxy = this._hubService.createProxy("gamesHub");

        this.registerOnServerEvents();

        this._hubService.onConnect(() => {
            this._userService.nameObservable.subscribe(name =>
                name ? this._proxy.invoke("AddClient", name) : null);

            //this._proxy = this._hubService.createProxy("gamesHub");
        });
    }

    public play(move: Move) {
        this._proxy.invoke("Play", move);
    }

    // TEST
    public clearPlayers() {
        this._proxy.invoke("ClearPlayers");
    }
    // TEST

    public challenge(player: Player) {
        this._proxy.invoke("Challenge", player);
    }

    public acceptChallenge(player: Player) {
        this._proxy.invoke("AcceptChallenge", player);
    }

    public declineChallenge(player: Player) {
        this._proxy.invoke("DeclineChallenge", player);
    }

    public getPlayers() {
        this._proxy.invoke("GetPlayers");
    }
    
    private registerOnServerEvents() {
        this._proxy.on("Play", (move: Move) => {
            this._opponentPlaySubject.next(move);
        });

        this._proxy.on("ChallengeAccepted", (player: Player) => {
            this._challengeAcceptedSubject.next(player);
        });

        this._proxy.on("ChallengeDeclined", () => {
            this._challengeDeclinedSubject.next();
        });

        this._proxy.on("Challenged", (player: Player) => {
            this._challengedSubject.next(player);
        });

        this._proxy.on("PlayersUpdated", players => {
            var others = players.filter(x => x.ConnectionToken != this._token);
            this._playersUpdatedSubject.next(others);
        });

        this._proxy.on("TokenGenerated", token =>
            this._token = token);
    }
}