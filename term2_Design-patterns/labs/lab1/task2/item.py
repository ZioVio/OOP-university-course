class Item:

    id = 0

    def __init__(self, name: str, start_price: float, max_price: float):
        Item.id += 1
        self.id = Item.id
        self.name = name
        self.start_price = start_price
        self.current_price = start_price
        self.max_price = max_price

    def reset_price(self):
        self.current_price = self.start_price