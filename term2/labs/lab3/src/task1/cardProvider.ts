import { Card, CardSuit, CardRank } from './card';

export class CardProvider {

    private static _instance: CardProvider;

    private cachedCards: Card[] = [];

    public getCard(rank: CardRank, suit: CardSuit) {
        const card = this.cachedCards.find(c => c.rank === rank && c.suit === suit);
        if (!card) 
            return null;
        return card.clone();
    }

    public static get instance(): CardProvider {
        if (!CardProvider._instance) {
            CardProvider._instance = new CardProvider();
        }
        return CardProvider._instance;
    }

    public loadCards(): void {
        const ranks = Object.values(CardRank).map(v => parseInt(v as string)).filter(v => !isNaN(v));
        const suits = Object.values(CardSuit).map(v => parseInt(v as string)).filter(v => !isNaN(v));
        for (const rank of ranks) {
            for (const suit of suits) {
                this.cachedCards.push(new Card({ rank, suit }));
            }
        }

        // just for demo
        console.log(this.cachedCards, this.cachedCards.length);
    }
}