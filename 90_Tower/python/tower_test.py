import unittest
import tower

class MyTestCase(unittest.TestCase):
    def test_something(self):
        t = tower.Tower()
        self.assertTrue(t.empty())

        d = tower.Disk(3)
        t.add(d)
        self.assertFalse(t.empty())

        d5 = tower.Disk(5)
        self.assertRaises(Exception, t.add, d5)
        self.assertFalse(t.empty())

    def test_oksize(self):
        t = tower.Tower()
        self.assertTrue(t.empty())

        d5 = tower.Disk(5)
        t.add(d5)
        self.assertFalse(t.empty())

        d3 = tower.Disk(3)
        t.add(d3)
        self.assertFalse(t.empty())

        self.assertEqual(t.top(), d3)
        self.assertEqual(t.pop(), d3)
        self.assertEqual(t.pop(), d5)

    def test_game(self):
        g = tower.Game()
        self.assertEqual(g.moves(), 0)
        self.assertFalse(g.winner())

    def test_format(self):
        t = tower.Tower()
        d3 = tower.Disk(3)
        d5 = tower.Disk(5)
        t.add(d5)
        t.add(d3)

        f = t.vertical_format(6, 3)
        self.assertEqual(f, ['      ', '[ 3 ] ', '[ 5 ] '])

if __name__ == '__main__':
    unittest.main()
