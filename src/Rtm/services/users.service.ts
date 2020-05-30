class UsersService {
    user = new Map<string, SocketIO.Socket>();

    addConnection = (id: string, sock: SocketIO.Socket) => {
        this.user.set(id, sock)
    }

    removeConnection = (id: string) => {
        this.user.delete(id)
    }

}

export const usersService = new UsersService()