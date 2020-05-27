export const usersService = {
    user: new Map<string, SocketIO.Socket>(),

    addConnection: (id: string, sock: SocketIO.Socket) => {
        usersService.user.set(id, sock)
    },

    removeConnection: (id: string) => {
        usersService.user.delete(id)
    }
};