from interop_lab import InteropPy

class NewList(InteropPy):
  def Mean(self):
    print("Running own mean")
    sum = 0
    for i in range(len(self)):
      sum += self[i]
    return sum / len(self)
    #return sum(self + self) / len(self)

  def RMS(self):
    print("Running own RMS")
    #sum = 0
    #for i in range(len(self)):
    #  sum += self[i] * self[i]
    return (self ** 2).Mean() ** 0.5
