import random
import math

"""
    向量a * 向量b = |a|*|b| * cos@

    平方根公式
    x = -b (+-) sqrt(b^2 - 4ac) / (2 * a)

    description: 主要用于生成任意的两点之间的间距大于定长，用于离散坐标位置生成
"""

class RadomPoints:
    """docstring for RadomPoints"""
    def __init__(self,dis,firstcount,count,step):

        self.dis = dis
        self.total = count
        self.step = step
        self.firstcount = firstcount
        self.point_list = []


    def randomRadius(self,dis,count):

        delt = 2.0 * math.pi /count

        radius = dis/2.0 * math.sin(delt/2.0)

        return radius , delt


    def randomPosition(self,r):

        radius = math.ceil(r)

        x = random.randint(-radius,radius)

        d = random.random()

        x = x - d if x>=0 else x + d

        y = math.sqrt( math.pow(radius,2) - math.pow(x,2))

        return x,y


    def getNextPosition(self,x,y,delt):

        a = math.pow(x,2) + math.pow(y,2)

        n = a *  math.cos(delt)

        b = -2.0 * n * y

        c = math.pow(n,2) - math.pow(x,2) * a

    #    print("n = %f" % n)

        y1,y2 = self.getSquareRoot(a,b,c)

        if y1 == y2 and y1 == 0 :

            return []

        y1 = max(y1,y2) if x >= 0 else min(y1,y2)

        x1 = (n - y1 * y) / x if y1 != y2 else math.sqrt(a - math.pow(y1,2))

        positions = [(round(x1,4),round(y1,4))]

        return positions



    def getSquareRoot(self,a,b,c):

        condition = math.pow(b,2) - 4.0 * a * c

        if condition < 0:

            return 0.0,0.0

        x1 = (-b + math.sqrt(condition)) / (2.0 * a)

        x2 = (-b - math.sqrt(condition)) / (2.0 * a)

        return x1,x2


    def getRoundPosition(self,radius,delt,count):

    #    radius,delt =  randomRadius(dis,count)

        x,y = self.randomPosition(radius)

        point_list = []

        while( len(point_list) < count):

            positions = self.getNextPosition(x,y,delt)

            if len(positions) > 0:

                x,y = positions[0]

                point_list.extend(positions)

            else:

                break

        return point_list

    def getRoundPoint(radius,delt):

        x0,y0 = randomPosition(radius)

        count = math.floor(2.0 * math.pi / delt)

        a = math.asin(x0/radius)

        point_list = []

        while( len(point_list) < count ):

            if a > 2.0 * math.pi:
                a -= 2.0 * math.pi

#            print("a = %.2f,radius = %.2f,delt = %.2f, count = %i" % (a,radius,delt,count))
            x,y = radius * math.sin(a) , radius * math.cos(a)

    #        print(x,y)

            point_list.append((round(x,2),round(y,2)))

            a += delt

        return point_list


    def getPoints(dis,firstcount,total):

        radius,delt = randomRadius(dis,firstcount)

        point_list = []

        while(len(point_list) < total):

            round_points = getRoundPoint(radius,delt)

            if len(round_points) > 0:

                point_list.extend(round_points)

            else:

                pass

            radius += dis

            delt = 2.0 * math.asin(dis / (2.0 *radius))

        return point_list

    def process(self):

        self.point_list = self.getPoints(self.dis,self.firstcount,self.total)

        self.point_list = list(set(self.point_list))



if __name__ == '__main__':

    points = RadomPoints(10,4,10,1)

    points.process()

    print(points.point_list)













