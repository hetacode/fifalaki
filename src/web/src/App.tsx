import React from 'react';
import logo from './logo.svg';
import './App.css';
import { RecoilRoot } from "recoil";
import MainPage from './pages/MainPage/main.page';
import MasterGamePage from './pages/MasterGamePage/master-game.page';

function App() {
  return (
    <RecoilRoot>
      <div className="App">
        <header className="App-header">
          <MasterGamePage/>
          {/* <MainPage /> */}
        </header>
      </div>
    </RecoilRoot>
  );
}

export default App;
