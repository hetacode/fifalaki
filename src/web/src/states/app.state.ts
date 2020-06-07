import { atom } from "recoil";

export const appState = atom<IAppState>({
    key: "appState",
    default: {
        connectionId: ""
    }
})

export interface IAppState {
    connectionId: string
}