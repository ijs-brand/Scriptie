library(ggplot2)
library(dplyr)
library(scales)

# Lees de data in van Q-Learning zonder HashSet.
qlearning <- read.csv(("C:/Users/Ysbra/Scriptie-Copy/Q-Learning-eind-resultaat2.txt"))
#qlearning <- distinct(qlearning)
names(qlearning) <- c("frame", "gen", "ronde", "poging")
# sorteert de frames
#qlearning$frame <- sort(qlearning$frame)
# maakt een plot
plot(qlearning$frame, ylim=c(0,60000), xlim=c(0,55))


qlearningmet <- read.csv(("C:/Users/Ysbra/Scriptie/Q-Learning-eind-resultaat.txt"))
#qlearningmet <- distinct(qlearningmet)
names(qlearningmet) <- c("frame", "gen", "ronde", "poging")
#qlearningmet$frame <- sort(qlearningmet$frame)
points( qlearningmet$frame, col="red")

neuroevolutie <- Neuroevolution.eindresultaat <- read.csv("C:/Users/Ysbra/Scriptie/Neuroevolution-eindresultaat.txt", header=FALSE)
names(neuroevolutie) <- c("frame", "gen", "ronde", "poging")
#neuroevolutie$frame <- sort(neuroevolutie$frame)
points( neuroevolutie$frame, col="green")
neuroeindresultaat <- neuroevolutie

# Meet het verschil tussen Hash-Q-Learning en single agent Q-Learning
t.test(qlearningmet$frame, neuroevolutie$frame)
t.test(qlearning$frame, qlearningmet$frame)

sd(qlearning$frame)
sd(qlearningmet$frame)

sd(neuroevolutie$frame)
sd(qlearningmet$frame)


### ------------------------------------------------------------------ 

# Q-Learning
QMean <- mean(qlearning$frame)
QSD <- sd(qlearning$frame)
# Hash-Q-Learning
HashMean <- mean(qlearningmet$frame)
HashSD <- sd(qlearningmet$frame)
# Neuroevolution
NeuroMean <- mean(neuroevolutie$frame)
NeuroSD <- sd(neuroevolutie$frame)

x <- seq(0, 70000, length=2000)
y <- dnorm(x, mean=HashMean, sd=HashSD)
z <- dnorm(x, mean=QMean, sd=QSD)
w <- dnorm(x, mean=NeuroMean, sd=NeuroSD)
plot(x, y, type="l", lwd=1, col="blue")
lines(x, z)
lines(x, w)

### ---------------------------------------------------------


qlearningmetdata <- read.csv("C:/Users/Ysbra/Scriptie/Q-Learning_extra_data2.txt")
qlearningmetdata <- distinct(qlearningmetdata)
names(qlearningmetdata) <- c("frame", "gen", "totale afstand", "gemiddelde afstand", "gemiddelde afstand verschil", "aantal ronden", "gemiddeldedist")

neurodata <- read.csv("C:/Users/Ysbra/Scriptie/Neuroevolution-extra_data.txt")
neurodata <- distinct(neurodata)
names(neurodata) <- c("frame", "gen", "aantalronden")


#plot(neurodata$frame, neurodata$aantalronden, type="p", ylim=c(0,11))


Q.Learning_extra_data <- read.csv("C:/Users/Ysbra/Scriptie/Q-Learning_extra_data.txt")
Q.Learning_extra_data <- distinct(Q.Learning_extra_data)
names(Q.Learning_extra_data) <- c("frame", "gen", "totale afstand", "gemiddelde afstand", "gemiddelde afstand verschil", "aantal ronden", "gemiddeldedist")

#plot(Q.Learning_extra_data$frame, Q.Learning_extra_data$`aantal ronden`, xlim=c(0,70000), ylim=c(0,11))
#points(qlearningmetdata$frame, qlearningmetdata$`aantal ronden`, col="red", xlim=c(0,70000), ylim=c(0,11))


NeuroMean <- aggregate(neurodata[, 1], list(neurodata$aantalronden), mean)
NeuroSD <- aggregate(neurodata[, 1], list(neurodata$aantalronden), sd)

NeuroStatistiek <- cbind(NeuroMean, NeuroSD[2])
names(NeuroStatistiek) <- c("aantalronden","NeuroMean","NeuroSd")

NeuroMin <- NeuroStatistiek$NeuroMean - NeuroStatistiek$NeuroSd
NeuroPlus <- NeuroStatistiek$NeuroMean + NeuroStatistiek$NeuroSd
NeuroPlus[1] <- 0
NeuroMin[1] <- 0
Neuroevolution <- cbind(NeuroStatistiek, NeuroPlus, NeuroMin)

QlearningMean <- aggregate(Q.Learning_extra_data[, 1], list(Q.Learning_extra_data$`aantal ronden`), mean)
QLearningSD <- aggregate(Q.Learning_extra_data[, 1], list(Q.Learning_extra_data$`aantal ronden`), sd)

QLearningInfo <- NA
QLearningInfo <- cbind(QlearningMean,QLearningSD[2])
names(QLearningInfo) <- c("aantalronden","mean","sd")

SDmin <- QLearningInfo$mean - QLearningInfo$sd
SDplus <- QLearningInfo$mean + QLearningInfo$sd
SDplus[1] <- 0
SDmin[1] <- 0
QLearning <- cbind(QLearningInfo, SDmin, SDplus)

HashQMean <- aggregate(qlearningmetdata[, 1], list(qlearningmetdata$`aantal ronden`), mean)
HashQSD <- aggregate(qlearningmetdata[, 1], list(qlearningmetdata$`aantal ronden`), sd)

QHashInfo <- cbind(HashQMean,HashQSD[2])
names(QHashInfo) <- c("AantalRonden","HashMean","HashSd")
HashSDmin <- QHashInfo$HashMean - QHashInfo$HashSd
HashSDplus <- QHashInfo$HashMean + QHashInfo$HashSd
HashQLearning <- cbind(QHashInfo, HashSDmin, HashSDplus)

HashSDplus[1] <- 0
HashSDmin[1] <- 0
HashSD[1] <- 0

Total <- cbind(HashQLearning, QLearning)
Total <- cbind(Total, Neuroevolution)


x <- Total$AantalRonden
# bovenste lijn
y1 <- Total$HashSDmin
# onderste lijn
y2 <- Total$HashSDplus
# middelste lijn
y3 <- Total$HashMean
# bovenste lijn
y4 <- Total$SDmin
# onderste lijn
y5 <- Total$SDplus
# middelste lijn
y6 <- Total$mean
# bovenste lijn
y7 <- Total$NeuroMin
# onderste lijn
y8 <- Total$NeuroPlus
# middelste lijn
y9 <- Total$NeuroMean

# plot
plot(y1,x,type="l",bty="L",xlab="Number of frames",ylab="Number of rounds", xlim=c(0,42000), col="white", main="Neuroevolution vs. Hash Q-Learning")
# voegt onderste lijn toe
blauw <- rgb(155, 150, 205, 255, maxColorValue=400)
redtrans <- rgb(0, 255, 20, 255, maxColorValue=400)
polygon(c(y4,rev(y5)),c(x,rev(x)),col=redtrans, border=NA)
points(y6,x,type="l",col="green", lwd=3)
orange <- rgb(250, 50, 0, 170, maxColorValue=370)
polygon(c(y7,rev(y8)),c(x,rev(x)),col=orange, border=NA)
points(y9,x,type="l",col="orange", lwd=3)

# plot
plot(y1,x,type="l",bty="L",xlab="Number of frames",ylab="Number of rounds", xlim=c(0,50000), col="white", main="Hash Q-Learning vs. Q-Learning")
# voegt onderste lijn toe
grijs <- rgb(150, 150, 150, 150, maxColorValue=400)
orange <- rgb(199,21,133, 150, maxColorValue=370)
polygon(c(y1,rev(y2)),c(x,rev(x)),col=grijs, border=NA)
points(y3,x,type="l",col="grey41", lwd=1)
redtrans <- rgb(0, 255, 20, 255, maxColorValue=400)
polygon(c(y4,rev(y5)),c(x,rev(x)),col=orange, border=NA)
points(y6,x,type="l",col="blue", lwd=2)

legend(20000, 3, 
       legend = c("Hash Q-Learning", "Q-Learning"), 
       col = c(orange, 
               grijs), 
       pch = c(19,19), 
       bty = "d", 
       pt.cex = 1.2, 
       cex = 0.9, 
       text.col = "black", 
       horiz =  F, 
       inset = c(0.2, 0.1),
)


MeanHash <- mean(Total$HashMean)
MeanQ <- mean(Total$mean)

Qsamen <- cbind(MeanHash, MeanQ)

barplot(MeanHash)

mean(qlearning$frame)

df <- data.frame(bar = c(mean(qlearningmet$frame), mean(neuroeindresultaat$frame)),
                 error = c(sd(qlearningmet$frame), sd(neuroeindresultaat$frame)))

t.test(qlearningmet$frame,neuroeindresultaat$frames)

foo <- barplot(df$bar,ylim=c(0,50000), main="Hash Q-Learning vs. Neuroevolution", name=c("Hash Q-Learning","Neuroevolution"), ylab="Number of frames", col="#69b3a2", width=c(0.1, 0.1))
arrows(x0=foo,y0=df$bar+df$error,y1=df$bar-df$error,angle=90,code=3,length=0.1)

###### -------------------------------------

