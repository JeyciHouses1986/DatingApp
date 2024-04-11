export interface Message {
    id: number
    sederId: number
    senderUsername: string
    senderPhotoUrl: string
    recipientId: number
    recipientUsername: string
    recipientPhotoUrl: string
    content: string
    dateRead: Date
    messageSent: string
  }