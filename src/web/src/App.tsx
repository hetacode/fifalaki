import React from 'react';
import logo from './logo.svg';
import './App.css';
import { RecoilRoot } from "recoil";
import MainPage from './pages/MainPage/main.page';

function App() {
  return (
    <RecoilRoot>
      <div className="App">
        <header className="App-header">
          <MainPage />
        </header>
      </div>
    </RecoilRoot>
  );
}

export default App;
