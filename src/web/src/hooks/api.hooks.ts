import { useSetRecoilState } from "recoil";
import { gamesListState } from "../recoil/states/games-list.state"
import axios from "axios"

export function useGetGamesList() {
    const setGamesList = useSetRecoilState(gamesListState)

    const get = async () => {
        let res = await axios.get(`${process.env.REACT_APP_API_ENDPOINT}/api/games/list`)
        console.log(res.data)
        // TODO: same to recoil state
    }
    return get
}