class Block:
    def __init__(self, id=0, ready=False):
        self.id = id
        self.ready = ready


class Stack:
    def __init__(self, id=0, max_height=8):
        self.id = id
        self.max_height = max_height
        self.blocks = list()


class WorldState:
    def __init__(self):
        self.production = Stack(0, 4)
        self.buffers = []
        self.buffers.append(Stack(1))
        self.buffers.append(Stack(2))
        self.buffers.append(Stack(3))
        self.buffers.append(Stack(4))
        self.buffers.append(Stack(5))
        self.buffers.append(Stack(6))
        self.handover = Stack(7, 1)

        #region block init
        b37 = Block()
        b37.id = 37
        b37.ready = False
        self.production.blocks.append(b37)

        b36 = Block()
        b36.id = 36
        b36.ready = False
        b35 = Block()
        b35.id = 35
        b35.ready = True
        b12 = Block()
        b12.id = 12
        b12.ready = False
        b8 = Block()
        b8.id = 8
        b8.ready = False
        b5 = Block()
        b5.id = 5
        b5.ready = False
        self.buffers[0].blocks.extend([b36, b35, b12, b8, b5])

        b30 = Block()
        b30.id = 30
        b30.ready = False
        b28 = Block()
        b28.id = 28
        b28.ready = False
        b18 = Block()
        b18.id = 18
        b18.ready = False
        b17 = Block()
        b17.id = 17
        b17.ready = False
        b14 = Block()
        b14.id = 14
        b14.ready = False
        b9 = Block()
        b9.id = 9
        b9.ready = False
        b2 = Block()
        b2.id = 2
        b2.ready = False
        self.buffers[1].blocks.extend([b30, b28, b18, b17, b14, b9, b2])

        b23 = Block()
        b23.id = 23
        b23.ready = False
        b16 = Block()
        b16.id = 16
        b16.ready = False
        b1 = Block()
        b1.id = 1
        b1.ready = False
        self.buffers[2].blocks.extend([b23, b16, b1])

        b33 = Block()
        b33.id = 33
        b33.ready = True
        b29 = Block()
        b29.id = 29
        b29.ready = False
        b26 = Block()
        b26.id = 26
        b26.ready = False
        b25 = Block()
        b25.id = 25
        b25.ready = True
        b21 = Block()
        b21.id = 21
        b21.ready = False
        b19 = Block()
        b19.id = 19
        b19.ready = False
        b11 = Block()
        b11.id = 11
        b11.ready = False
        b4 = Block()
        b4.id = 4
        b4.ready = False
        self.buffers[3].blocks.extend([b33, b29, b26, b25, b21, b19, b11, b4])

        b34 = Block()
        b34.id = 34
        b34.ready = True
        b24 = Block()
        b24.id = 24
        b24.ready = False
        b20 = Block()
        b20.id = 20
        b20.ready = False
        b15 = Block()
        b15.id = 15
        b15.ready = False
        b6 = Block()
        b6.id = 6
        b6.ready = False
        self.buffers[4].blocks.extend([b34, b24, b20, b15, b6])

        b32 = Block()
        b32.id = 32
        b32.ready = False
        b31 = Block()
        b31.id = 31
        b31.ready = False
        b27 = Block()
        b27.id = 27
        b27.ready = False
        b22 = Block()
        b22.id = 22
        b22.ready = False
        b13 = Block()
        b13.id = 13
        b13.ready = False
        b10 = Block()
        b10.id = 10
        b10.ready = False
        b7 = Block()
        b7.id = 7
        b7.ready = False
        b3 = Block()
        b3.id = 3
        b3.ready = False
        self.buffers[5].blocks.extend([b32, b31, b27, b22, b13, b10, b7, b3])
        #endregion after block init

    def move(self, sourceId, targetId) -> bool:
        if sourceId == self.production.id:
            if len(self.production.blocks) > 0:
                b = self.production.blocks.pop()
        else:
            for buffer in self.buffers:
                if buffer.id == sourceId:
                    if len(buffer.blocks) > 0:
                        b = buffer.blocks.pop()

        if "b" not in locals():
            return False

        if targetId == self.handover.id:
            if len(self.handover.blocks) == 0:
                self.handover.blocks.append(b)
                return True
        else:
            for buffer in self.buffers:
                if buffer.id == targetId and len(buffer.blocks) < buffer.max_height:
                    buffer.blocks.append(b)
                    return True

        return False
