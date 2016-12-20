import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule }   from '@angular/router';
import { FormsModule }   from '@angular/forms';

import { AppComponent } from './app.component';
import { StartComponent }  from './app.start.component';
import { LobbyComponent }  from './app.lobby.component';
import { TicTacToeComponent } from "./app.tictactoe.component";

import { HubService } from "./services/hub.service";
import { UserService } from "./services/user.service";
import { GameService } from "./services/game.service";
import { ChatService } from "./services/chat.service";

@NgModule({
    imports: [
        BrowserModule,
        RouterModule.forRoot([
            { path: 'tictactoe/:opponentConnectionToken/:isChallenger', component: TicTacToeComponent },
            { path: "lobby", component: LobbyComponent },
            { path: '', component: StartComponent },
            //{ path: '**', component: PageNotFoundComponent }
        ]),
        FormsModule
    ],
    declarations: [
        AppComponent,
        StartComponent,
        LobbyComponent,
        TicTacToeComponent
    ],
    providers: [
        HubService,
        UserService,
        GameService,
        ChatService
    ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }