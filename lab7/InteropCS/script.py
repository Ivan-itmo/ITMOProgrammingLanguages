import math
from interop_lab import InteropPy

class NewList(InteropPy):
  def Mean(self):
    print("Running own mean")
    #sum = 0
    #for i in range(len(self)):
    #  sum += self[i]
    #return sum / len(self)
    return sum(self + self) / len(self)

  def RMS(self):
    print("Running own RMS")
    # Bad solution
    #sum = 0
    #for i in range(len(self)):
    #  sum += self[i] ** 2
    
    #return math.sqrt(sum / len(self))
    # Good souluion should use
    return (self ** 2).Mean() ** 0.5
