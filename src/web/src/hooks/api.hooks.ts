import { useSetRecoilState } from "recoil";
import { gamesListState } from "../states/games-list.state"
import axios from "axios"

export function useGetGamesList() {
    const setGamesList = useSetRecoilState(gamesListState)

    const get = async () => {
        let res = await axios.get(`${process.env.REACT_APP_API_ENDPOINT}/api/games/list`)
        setGamesList(res.data.items)
    }
    return get
}